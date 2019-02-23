import { ISpeechRecognitionService } from "../models/interfaces/ISpeechRecognitionService";
import { IWindow } from "../models/interfaces/IWindow";
import { NGXLogger } from "ngx-logger";
import { VoiceActivityDetectionService } from "./voice-activity-detection.service";

declare var $, Resampler: any;
const { AudioContext, webkitAudioContext, webkitSpeechRecognition }: IWindow = <IWindow>window;

export class SpeechRecognitionCloudService implements ISpeechRecognitionService {

    isVoiceDetected: boolean;
    isMicDetected: boolean;
    micActivityDetectionService: VoiceActivityDetectionService;
    voiceActivityDetectionService: VoiceActivityDetectionService;
    wsServerAddress = process.env.WS_SERVER_ADDRESS;
    IsVadDetected = false;
    micWasInitialized = false;
    processor: any;

    constructor(private socket: WebSocket, private logger: NGXLogger, private currentEmployee) {
        this.logger.info('---==={ Starting Google Speech API }===---');
        let CustomAudioContext = AudioContext || webkitAudioContext;
        this.micActivityDetectionService = new VoiceActivityDetectionService(200, 1000);
        this.voiceActivityDetectionService = new VoiceActivityDetectionService(200, 60000);

        navigator.mediaDevices.getUserMedia({ audio: true, video: false }).then((stream) => {
            let context = new CustomAudioContext();
            let source = context.createMediaStreamSource(stream);
            this.processor = context.createScriptProcessor(2048, 1, 1);


            source.connect(this.processor);
            this.processor.connect(context.destination);
            this.voiceActivityDetectionService.updateStatus();
            this.isVoiceDetected = true;
            this.voiceActivityDetectionService.StartManually(true);
            this.processor.onaudioprocess = (e) => {
                this.IsVadDetected = this.voiceActivityDetectionService.IsVadDetected(e.inputBuffer.getChannelData(0));
                let isMicActivity = this.micActivityDetectionService.IsVadDetected(e.inputBuffer.getChannelData(0));

                if (isMicActivity && !this.isMicDetected){
                    this.isMicDetected = true;
                    this.onMicActivated();
                }
                if (!isMicActivity && this.isMicDetected) {
                    this.isMicDetected = false;
                    this.onMicDisactivated();
                }
    
                if (this.socket.readyState == 1) {    
                    if (this.isVoiceDetected && !this.IsVadDetected) {
                        this.onStopRecording(this.socket);
                    }
    
                    if (!this.isVoiceDetected && this.IsVadDetected) {
                        this.onStartRecording(this.socket);
                    }
                }
    
                this.isVoiceDetected = this.IsVadDetected;
    
                if (this.socket.readyState == 1 && this.IsVadDetected) {
                    let rawData = e.inputBuffer.getChannelData(0);
    
                    // Resampling audio buffer from 48KHz to 16KHz                     
                    let resampler = new Resampler(48000, 16000, 1, rawData);
                    resampler.resampler(rawData.length)
                    let packedData = this.convertFloat32ToInt16(resampler.outputBuffer);
    
                    //console.log(`Sending ${packedData.byteLength} bytes`);
                    this.socket.send(packedData);
                }
            };
        });
    }

    public start() {
        this.logger.info('Recognition service is started');
        this.logger.info("Conected. Sending auth message");
        let connectionGuid = localStorage.getItem('connectionGuid');
        let data = { type: "Authorize", employeeId: this.currentEmployee.id, workingMode: "CloudSpeechApi", connectionGuid: connectionGuid };
        let json = JSON.stringify(data);
        this.socket.send(json);

        this.socket.onmessage = (event) => {
            this.logger.info(`RCVD: ${event.data}`);
            let serverData = JSON.parse(event.data);
    
            if (serverData.error !== "") {
                this.logger.error(serverData.error)
            }
            if (serverData.debug) {
                this.logger.info(serverData.debug);
            }
            
            this.onRecognized(parseInt(serverData.blockId))   
        };
    }
    public end(callback = () => { }) {
        this.logger.info('Recognition service is ended');
        this.socket.close();
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

    onStopRecording(socket) {
        this.logger.info("Stop recording.")
        this.onRecognitionSessionEnd();
        let data = { type: "stopRecording", stopRecording: true };
        let json = JSON.stringify(data);
        socket.send(json);
    }

    onStartRecording(socket) {
        this.logger.info("Start recording.")
        let data = { type: "startRecording", startRecording: true };
        let json = JSON.stringify(data);
        socket.send(json);
    }

    public convertFloat32ToInt16(buf32: any) {
        let elapsedBuf32 = buf32.length;
        let buf16 = new Uint16Array(buf32.length);
        for (var i = 0; i < buf32.length; i++) {
            buf16[i] = Math.min(1, buf32[i]) * 0x7FFF;
        }

        return buf16.buffer;
    }
}