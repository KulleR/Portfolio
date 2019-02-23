import { ISpeechRecognitionService } from "../models/interfaces/ISpeechRecognitionService";
import { NGXLogger } from "ngx-logger";
import { VoiceActivityDetectionService } from "./voice-activity-detection.service";
import { IWindow } from "../models/interfaces/IWindow";
import { Language } from "../models/enums/Language";

const { webkitSpeechRecognition }: IWindow = <IWindow>window;

export class SpeechRecognitionWebService implements ISpeechRecognitionService {

    isVoiceDetected: boolean;
    isMicDetected: boolean;
    micActivityDetectionModule: VoiceActivityDetectionService;
    voiceActivityDetectionModule: VoiceActivityDetectionService;
    wsServerAddress = process.env.WS_SERVER_ADDRESS;
    recognition: any;
    isStopedManually: boolean = false;
    isSupportedWebSpeechApi: boolean = true;

    constructor(private socket: any, private logger: NGXLogger, private currentUser) {
        this.micActivityDetectionModule = new VoiceActivityDetectionService(200, 1000);
        this.voiceActivityDetectionModule = new VoiceActivityDetectionService(200, 60000);
        this.logger.info('Preparing speech recognition module');

        if (window.hasOwnProperty('webkitSpeechRecognition')) {
            this.recognition = new webkitSpeechRecognition();
            this.recognition.continuous = false;
            this.recognition.interimResults = true;
            this.recognition.maxAlternatives = 30;
            this.recognition.lang = this.currentUser.language;

            this.socket.onmessage = (event) => {
                let serverData = JSON.parse(event.data);

                if (serverData.answerType == "Deauthorize") {
                    this.onDeauthorized();
                }

                if (serverData.answerType == "ConnectionGuid") {
                    this.logger.info("Setting guid = " + serverData.debug);
                    localStorage.setItem("connectionGuid", serverData.debug)
                }

                if (serverData.error !== "") {
                    this.logger.error(serverData.error)
                }

                if (serverData.debug) {
                    this.logger.info(serverData.debug);
                }

                if (parseInt(serverData.blockId) > 0){
                    if (serverData.isFinal){
                        this.logger.info(`wss => FinalComplete (block: "${serverData.blockName}")`);
                    } else{
                        this.logger.debug(`wss => InterimComplete (block: "${serverData.blockName}")`);
                    }
                }

                this.onRecognized(parseInt(serverData.blockId))
            };

            this.registerEvents();  
        }
        else {
            this.isSupportedWebSpeechApi = false;
            this.logger.info('Your browser is not supported Web Speech API.');
        }
    }

    public start() {
        if (!window.hasOwnProperty('webkitSpeechRecognition')) {
            this.onNotSupported();
        }

        this.logger.info('Starting webkitSpeechRecognition');
        let connectionGuid = localStorage.getItem('connectionGuid');
        let data = { type: "Authorize", employeeId: this.currentUser.id, workingMode: "WebSpeechApi", connectionGuid: connectionGuid };
        let json = JSON.stringify(data);
        this.socket.send(json);
        this.recognition.start();
    }

    public end(callback = () => { }) {
        this.logger.info('Stopping webkitSpeechRecognition');
        this.isStopedManually = true;
        this.socket.close();
        this.recognition.abort();
        callback();
    }

    public onMicActivated() {
    }

    public onMicDisactivated() {
    }

    public onDeauthorized() {
    }

    public onRecognized(blockId) {
    }

    public onRecognitionSessionEnd() {
    }

    public onNotSupported() {
    }

    private registerEvents() {
        this.recognition.onresult = (event) => {
            
            if (event.results.length == 0){
                this.logger.warn(`speech-api -> empty results`);
                return;
            }

            let topAlternative;
            let totalAlternatives;
            for(let result of event.results){
                var recognitionAlternatives = [];
                for (let alternative of result){
                    if (topAlternative == null || topAlternative.confidence > alternative.confidence){
                        topAlternative = alternative;
                    }
                    totalAlternatives++;
                    let confidence = Math.round(alternative.confidence*100)/100;
                    recognitionAlternatives.push(
                        { transcript: alternative.transcript, time: Date.now(), confidence: confidence })
                }    

                let connectionGuid = localStorage.getItem('connectionGuid');

                let wssRequest = {
                    type: "Transcription", 
                    recognitionAlternatives: recognitionAlternatives,
                    employeeId: this.currentUser.id, 
                    workingMode: "WebSpeechApi", 
                    connectionGuid: connectionGuid,
                    isRecognitionResultFinal: result.isFinal
                };
                let requestJson = JSON.stringify(wssRequest);
                this.socket.send(requestJson);    

                if (result.isFinal) {
                    this.logger.info(`wss <= [${result.length} alt] "${topAlternative.transcript}" (conf:${Math.round(topAlternative.confidence*100)/100})`);                    
                }
            }
        }

        this.recognition.onend = () => {  
            if (!this.isStopedManually) {
                this.recognition.abort();
                setTimeout(() => this.recognition.start(), 0);
            }
        }

        this.recognition.onspeechstart = () => {
            this.onMicActivated();
        }

        this.recognition.onspeechend = () => { 
            this.onMicDisactivated();
        }
    }

}