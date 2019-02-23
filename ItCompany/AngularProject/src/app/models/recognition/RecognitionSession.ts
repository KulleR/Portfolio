import { RecognitionResult } from "./RecognitionResult";

export class RecognitionSession {
    public Recognitions: RecognitionResult [] = [];
    public StartDate: Date;
    public EndDate: Date;

    constructor() { }

    start() {
        this.Recognitions = [];
        this.StartDate = new Date();
    }

    end() {
        this.EndDate = new Date();
    }
}