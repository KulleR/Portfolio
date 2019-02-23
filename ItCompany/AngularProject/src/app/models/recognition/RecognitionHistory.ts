import { RecognitionSession } from "./RecognitionSession";
import { RecognitionResult } from "./RecognitionResult";
import { ScriptBlock } from "../script/ScriptBlock";

export class RecognitionHistory {
    private Sessions: RecognitionSession[] = [];
    get lastSession(): RecognitionSession { return this.Sessions[0] }
    get getSessions(): RecognitionSession[] { return this.Sessions.sort((a, b) => a.StartDate > b.StartDate ? -1 : 1) }

    startNewSession() {
        let newSession = new RecognitionSession();
        newSession.start();
        this.Sessions.push(newSession);
    }

    endLastSesion() {
        this.lastSession.end();
    }

    addRecognition(block: ScriptBlock){
this.lastSession.Recognitions.push(new RecognitionResult(block))
    }
}