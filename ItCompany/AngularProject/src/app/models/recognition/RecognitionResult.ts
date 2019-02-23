import { ScriptBlock } from "../script/ScriptBlock";

export class RecognitionResult {

    public IsManually: boolean;
    public Block: ScriptBlock;

    constructor(block: ScriptBlock) {
        this.IsManually = block.manuallyRecognized;
        this.Block = block;
    }
}