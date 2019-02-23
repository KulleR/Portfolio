import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { NGXLogger } from 'ngx-logger';
import { Observable } from 'rxjs';
import { map, catchError, flatMap } from 'rxjs/operators';
import { ScriptBlock } from '../models/script/ScriptBlock';
import { Router } from '@angular/router';
import { ScriptBlockForUpdate } from '../models/script/ScriptBlockForUpdate';

let config = require('./../../../config/web.config.json');

@Injectable({
  providedIn: 'root'
})
export class ScriptBlockService {
  readonly apiHost = config.api.url;
  scriptBlocks: ScriptBlock[];
  constructor(private http: HttpClient, private logger: NGXLogger, private router: Router) { }

  getScriptBlocks(scriptId): Observable<ScriptBlock[]> {
    this.logger.info("Getting script blocks...");
    return this.http.get<ScriptBlock[]>(this.apiHost + `/api/scripts/${scriptId}/blocks`).pipe(
      map(res => res.map(b => new ScriptBlock(b.id, b.header, b.text, new RegExp(b.pattern, "ig"),
        {
          isFinal: b.isFinal, cardColorClasses: b.cardColorClasses,
          badgeColorClasses: b.badgeColorClasses, isRepeated: b.isRepeated,
          isBegin: b.isBegin, order: b.order
        }))),
      map(blocks => this.scriptBlocks = blocks));
  }

  update(scriptId: number, blockId: number, recognitionBlock: ScriptBlockForUpdate) {
    this.logger.info("Updating script block...");
    return this.http.put(this.apiHost + `/api/scripts/${scriptId}/blocks/${blockId}`, recognitionBlock);
  }

  updateList(scriptId: number, recognitionBlocks: ScriptBlockForUpdate[]) {
    this.logger.info("Updating script blocks...");
    return this.http.put(this.apiHost + `/api/scripts/${scriptId}/blocks`, recognitionBlocks);
  }

  delete(scriptId: number, blockId: number) {
    this.logger.info("Deleting script block...");
    return this.http.delete(this.apiHost + `/api/scripts/${scriptId}/blocks/${blockId}`);
  }

  create(scriptId: number, block: ScriptBlockForUpdate) {
    this.logger.info("Creating script block...");
    return this.http.post(this.apiHost + `/api/scripts/${scriptId}/blocks`, block);
  }
}
