import { HtmlService } from "../../services/html.service";

export class ScriptBlock {
    constructor(id, header, text, pattern: RegExp,
        { isFinal = false, cardColorClasses = "bg-success text-white",
            badgeColorClasses = "badge-success", isRepeated = false,
            isBegin = false, order = 0 } = {}) {
        this.id = id;
        this.header = header;
        this.text = HtmlService.decodeNewLines(text, "\r\n");
        this.pattern = pattern;
        this.isBegin = isBegin;
        this.isFinal = isFinal;
        this.cardColorClasses = cardColorClasses;
        this.badgeColorClasses = badgeColorClasses;
        this.isRepeated = isRepeated;
        this.order = order;
        this.recognized = false;
    }
    public id: number;
    public order: number;
    public header: string;
    public text: string;
    public pattern: RegExp;
    public recognized: boolean;
    public manuallyRecognized: boolean;
    public isBegin: boolean;
    public isFinal: boolean;
    public isRepeated: boolean;
    public cardColorClasses: string;
    public badgeColorClasses: string;
    public recognizedCount: number = 0;

    public Recognize(isManually: boolean = false){
        this.recognized = true;
        this.recognizedCount++;
        this.manuallyRecognized = isManually;

    }
}