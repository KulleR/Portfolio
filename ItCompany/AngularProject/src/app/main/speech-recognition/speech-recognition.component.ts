import { Component, OnInit, ChangeDetectorRef, ViewRef, AfterViewInit } from '@angular/core';
import { NGXLogger } from 'ngx-logger';
import '../../../../node_modules/resamplerjs/dist/resampler.min.js';
import { ScriptBlockService } from '../../services/script-block.service';
import { ScriptBlock } from '../../models/script/ScriptBlock';
import { RecognitionHistory } from '../../models/recognition/RecognitionHistory';
import { RecognitionResult } from '../../models/recognition/RecognitionResult';
import { UserService } from '../../services/user.service';
import { SpeechRecognitionMode } from '../../models/enums/SpeechRecognitionMode';
import { SpeechRecognitionWebService } from '../../services/speech-recognition-web.service';
import { SpeechRecognitionCloudService } from '../../services/speech-recognition-cloud.service';
import { ISpeechRecognitionService } from '../../models/interfaces/ISpeechRecognitionService';
import { ScriptBlockForUpdate } from '../../models/script/ScriptBlockForUpdate';
import { SortablejsOptions } from 'angular-sortablejs/dist';
import { HtmlService } from '../../services/html.service';

let config = require('./../../../../config/web.config.json');

declare var $: any;

@Component({
  selector: 'app-speech-recognition',
  templateUrl: './speech-recognition.component.html',
  styleUrls: ['./speech-recognition.component.css']
})

export class SpeechRecognitionComponent implements OnInit {

  socket: WebSocket;
  isMicDetected: boolean = false;
  isDialogStarted: boolean = false;
  selectedBlock: ScriptBlock;
  blocks: ScriptBlock[];
  recognitionHistory: RecognitionHistory = new RecognitionHistory();

  detectChangesBusy: boolean = false;
  recognition: any;
  isRecognitionSupported: boolean = true;
  wsServerAddress = config.socket.url;
  recognitionService: ISpeechRecognitionService;
  isCardEditing: boolean = false;

  sortableOptions: SortablejsOptions = {
    onUpdate: () => {

      let blocksForUpdate: ScriptBlockForUpdate[] = [];
      for (let b of this.blocks) {
        let scriptBlock = new ScriptBlockForUpdate();
        scriptBlock.Id = b.id;
        scriptBlock.Text = b.text;
        scriptBlock.Header = b.header;
        blocksForUpdate.push(scriptBlock)
      }

      this.scriptBlockService.updateList(this.userService.currentUser.scriptId, blocksForUpdate).subscribe(blocks => {
        this.recognitionService.end(() => {
          this.openSocket();
        })
        this.scriptBlockService.getScriptBlocks(this.userService.currentUser.scriptId).subscribe(blocks => {
          this.blocks = blocks.sort((a, b) => a.order > b.order ? 1 : -1);
        });
      })
    },
    animation: 150
  };

  constructor(private ref: ChangeDetectorRef, private logger: NGXLogger,
    private scriptBlockService: ScriptBlockService, private userService: UserService, private HtmlService: HtmlService) { }

  ngOnInit() {
    $(window).scroll(() => this.triggerCss($(".card-container")));

    this.onUpdatedUserClaims(() => {
      this.scriptBlockService.getScriptBlocks(this.userService.currentUser.scriptId).subscribe(blocks => {
        this.blocks = blocks.sort((a, b) => a.order > b.order ? 1 : -1);
      });

      this.openSocket();
    });
  }

  ngOnDestroy() {
    console.log("Socket closed");
    this.recognitionService.end(() => { });

  }

  getBlockColorClasses(block: ScriptBlock) {
    return block.recognized ? block.manuallyRecognized ? "bg-warning" : block.cardColorClasses : 'bg-light';
  }

  getBadgeColorClasses(result: RecognitionResult) {
    return result.IsManually ? "badge-warning" : result.Block.badgeColorClasses;
  }

  onRecognized(block: ScriptBlock, { text = "", isManually = false }) {
    if (text !== "") {
      this.logger.log(`Matched: ${text.match(block.pattern)}`);
    }

    if (block.isBegin && !this.isDialogStarted) {
      this.logger.info(`Dialog begin phrase located`);
      this.isDialogStarted = true;
    }

    if (!this.isDialogStarted) {
      this.logger.info(`Waiting for dialog begin phrase`);
      return;
    }

    if (isManually) {
      this.logger.log(`Block '${block.header}' recognized manually`);
    }

    block.Recognize(isManually);
    this.recognitionHistory.addRecognition(block);
    if (block.isFinal) {
      this.onRecognitionSessionEnd();
    }
  }

  onRecognitionSessionEnd() {
    this.logger.info(`Completing dialog`);
    this.isDialogStarted = false;
    this.blocks.forEach(b => b.recognized = false);

    this.recognitionHistory.endLastSesion();
    this.recognitionHistory.startNewSession();
  }

  onCardClick(block) {
    this.selectedBlock = block;
    this.ref.detectChanges();
    $("#card-info").modal();

    $("#card-info .modal-header .edit-activate-link").click((e) => {
      e.preventDefault();
      $("#block-header").focus();
    });

    $("#card-info .modal-body .edit-activate-link").click((e) => {
      e.preventDefault();
      $("#block-text").focus();
    });
  }

  recognizeManually(block: ScriptBlock) {
    this.onRecognized(block, { isManually: true })
  }

  prevBlock() {
    let selectedBlockIndex = this.blocks.lastIndexOf(this.selectedBlock);
    this.selectedBlock = this.blocks[--selectedBlockIndex];
  }

  nextBlock() {
    let selectedBlockIndex = this.blocks.lastIndexOf(this.selectedBlock);
    this.selectedBlock = this.blocks[++selectedBlockIndex];
  }

  logout() {
    $(window).unbind('scroll');
    this.recognitionService.end(() => { });
    this.userService.logout();
    this.socket.close();
  }

  onUpdatedUserClaims(callback) {
    this.userService.updateUserClaims().subscribe(user => {
      this.userService.currentUser = user;
      callback();
    })
  }

  safeDetectChanges() {
    if (this.detectChangesBusy) {
      return;
    }

    this.detectChangesBusy = true;
    setTimeout(() => {
      if (!(this.ref as ViewRef).destroyed) {
        this.ref.detectChanges();
      }
      this.detectChangesBusy = false;
    }, 250);
  }

  triggerCss(fixedElement) {
    var scrollTop = $(window).scrollTop();
    var containerTopOffset = $(fixedElement).parent().offset().top;

    if (scrollTop >= containerTopOffset) {
      $(".content").addClass("fix");
    } else {
      $(".content").removeClass("fix");
    }
  }

  openSocket() {
    this.socket = new WebSocket(this.wsServerAddress);
    this.logger.info(`Opening websocket connection to ${this.wsServerAddress}`);

    this.socket.onopen = () => {
      this.recognitionHistory.startNewSession();
      this.recognitionService =
        this.userService.currentUser.speechRecognitionMode == SpeechRecognitionMode.WebSpeechApi ?
          new SpeechRecognitionWebService(this.socket, this.logger, this.userService.currentUser) :
          new SpeechRecognitionCloudService(this.socket, this.logger, this.userService.currentUser);

      this.registerRecognitionServiceEvent();
      this.recognitionService.start();

    };

    this.socket.onclose = () => {

    };
  }

  registerRecognitionServiceEvent() {
    this.recognitionService.onMicActivated = () => {
      this.isMicDetected = true;
      this.safeDetectChanges();
    }

    this.recognitionService.onMicDisactivated = () => {
      this.isMicDetected = false;
      this.safeDetectChanges();
    }

    this.recognitionService.onDeauthorized = () => {
      this.logout();
    }

    this.recognitionService.onRecognized = (blockId) => {
      if (blockId > 0) {
        let recogizedBlock = this.blocks.filter(b => b.id == blockId)[0];
        if (recogizedBlock != undefined && (!recogizedBlock.recognized || recogizedBlock.isRepeated)) {
          this.onRecognized(recogizedBlock, {});
        }
      }
    }

    this.recognitionService.onRecognitionSessionEnd = this.onRecognitionSessionEnd;

    this.recognitionService.onNotSupported = () => {
      this.isRecognitionSupported = false;
      this.safeDetectChanges();
    };
  }

  saveBlock() {
    this.logger.info(`Block ${this.selectedBlock.id} saving...`)

    let newStrippedText = HtmlService.strip($("#block-text").html());
    let newHeader = $("#block-header").text();

    this.isCardEditing = false;
    let scriptBlock = new ScriptBlockForUpdate();
    scriptBlock.Id = this.selectedBlock.id;
    scriptBlock.Text = newStrippedText;
    scriptBlock.Header = newHeader;

    this.scriptBlockService.update(this.userService.currentUser.scriptId, this.selectedBlock.id, scriptBlock).subscribe(() => {
      this.logger.info(`Block success ${this.selectedBlock.id} saved`);

      $("#card-info").modal("hide");

      this.onRecognitionSessionEnd();

      this.selectedBlock.text = newStrippedText;
      this.selectedBlock.header = newHeader;

      this.recognitionService.end(() => {
        this.openSocket();
      });
    });
  }

  deleteBlock(block: ScriptBlock) {
    this.scriptBlockService.delete(this.userService.currentUser.scriptId, block.id).subscribe(() => {
      this.scriptBlockService.getScriptBlocks(this.userService.currentUser.scriptId).subscribe(blocks => {
        this.blocks = blocks.sort((a, b) => a.order > b.order ? 1 : -1);
        this.recognitionService.end(() => {
          this.openSocket();
        })
      });
      $("#card-info").modal("hide");
    });
  }
}
