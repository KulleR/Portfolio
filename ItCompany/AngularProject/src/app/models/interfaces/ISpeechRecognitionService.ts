export interface ISpeechRecognitionService {
    onRecognitionSessionEnd();

    onMicActivated(callback: () => any);
    onMicDisactivated(callback: () => any);
    onRecognized(blockId: number);
    onDeauthorized();
    onNotSupported();

    start();
    end(callback: () => any);
}