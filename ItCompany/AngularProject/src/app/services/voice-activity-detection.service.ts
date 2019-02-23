import { Injectable } from '@angular/core';


export class VoiceActivityDetectionService {

  //#region public properties

  public samplesInBuffer = 2048;
  public defaultRateHz = 48000;
  public dbTreshold = -30;
  public maxTimeBeforeVoiceIsDetectedMs;
  public maxTimeBeforeSilenceIsDetectedMs; //1 min 

  //#endregion 

  //#region private properties

  private timeBetweenFramesMs = this.samplesInBuffer / this.defaultRateHz * 1000;
  private maxVoiceFramesBeforeVoiceIsDetected;
  private maxSilenceFramesBeforeSilenceIsDetected;
  private currentVoiceFrames = 0;
  private currentSilenceFrames = 0;

  private voiceIsDetected = false;
  //#endregion


  constructor(maxTimeBeforeVoiceIsDetectedMs: number = 200, maxTimeBeforeSilenceIsDetectedMs: number = 60000) {
    this.maxTimeBeforeVoiceIsDetectedMs = maxTimeBeforeVoiceIsDetectedMs;
    this.maxTimeBeforeSilenceIsDetectedMs = maxTimeBeforeSilenceIsDetectedMs;

    this.maxVoiceFramesBeforeVoiceIsDetected = this.maxTimeBeforeVoiceIsDetectedMs / this.timeBetweenFramesMs;
    this.maxSilenceFramesBeforeSilenceIsDetected = this.maxTimeBeforeSilenceIsDetectedMs / this.timeBetweenFramesMs;
  }

  updateStatus() {
    // console.log(`Updated!`);
  }

  public StartManually(voiceIsDetected) {
    this.voiceIsDetected = voiceIsDetected;
    if (voiceIsDetected) {
      this.currentSilenceFrames = 0;
    }
    else {
      this.currentVoiceFrames = 0;
    }
  }

  public IsVadDetected(samplesBuffer) {
    let frameVolumeDb = this.CalculateFrameVolume(samplesBuffer);

    if (frameVolumeDb > this.dbTreshold) {
      this.currentSilenceFrames = 0;
    }
    else {
      this.currentVoiceFrames = 0;
    }

    if (!this.voiceIsDetected) {
      if (frameVolumeDb > this.dbTreshold) {
        this.currentVoiceFrames++;
        if (this.currentVoiceFrames >= this.maxVoiceFramesBeforeVoiceIsDetected) {
          this.voiceIsDetected = true;
        }
      }
    }
    else {
      if (frameVolumeDb < this.dbTreshold) {
        this.currentSilenceFrames++;
        if (this.currentSilenceFrames >= this.maxSilenceFramesBeforeSilenceIsDetected) {
          this.voiceIsDetected = false;
        }
      }
    }

    return this.voiceIsDetected;
  }

  private CalculateFrameVolume(buf: any) {
    let sum = 0;
    let bufLength = buf.length;

    for (let i = 0; i < bufLength; i++) {
      sum += buf[i] * buf[i];
    }

    let rms = Math.sqrt(sum / bufLength);
    let decibel = 20 * Math.log10(rms);
    return Math.round(decibel);
  }
}