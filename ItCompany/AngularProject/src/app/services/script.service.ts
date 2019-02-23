import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { NGXLogger } from 'ngx-logger';
import { Observable } from 'rxjs';
import { map, catchError, flatMap } from 'rxjs/operators';
import { ScriptBlock } from '../models/script/ScriptBlock';
import { Router } from '@angular/router';
import { UserService } from './user.service';

let config = require('./../../../config/web.config.json');

@Injectable({
  providedIn: 'root'
})
export class ScriptService {
  readonly apiHost = config.api.url;
  recognitionBlocks: ScriptBlock[];
  constructor(private http: HttpClient, private logger: NGXLogger, private router: Router, private userService: UserService) { }

//   get(): Observable<ScriptBlock[]> {
//     this.logger.info("Getting recognition blocks");
//     return this.http.get<ScriptBlock[]>(this.apiHost + `/api/scripts/${this.userService.currentUser.sriptId}`).pipe(
//       map(res => res.map(b => new ScriptBlock(b.id, b.header, b.text, new RegExp(b.pattern, "ig"),
//         {
//           isFinal: b.isFinal, cardColorClasses: b.cardColorClasses,
//           badgeColorClasses: b.badgeColorClasses, isRepeated: b.isRepeated,
//           isBegin: b.isBegin, order: b.order
//         }))),
//       map(blocks => this.recognitionBlocks = blocks));
//   }
}
