import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { NGXLogger } from 'ngx-logger';
import { Observable } from 'rxjs';

let config = require('./../../../config/web.config.json');

@Injectable({
  providedIn: 'root'
})
export class UserService {
  static readonly userToken:string = 'userToken';
  returnUrl: string;
  public currentUser: any;
  readonly apiHost = config.api.url;
  constructor(private http: HttpClient, private router: Router, private logger: NGXLogger) { }

  userAuthentication(username, password, successCallback = (data: any) => {}, errorCallback = (err: HttpErrorResponse) => { }) {
    var data = { username, password, grant_type: 'password' };
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/json ', 'No-Auth': 'True' });
    this.http.post(this.apiHost + '/token/employee', data, { headers: reqHeader }).subscribe((data: any) => {
      sessionStorage.setItem('userToken', data.access_token);
      this.currentUser = data.user;
      successCallback(data);
      
      if (this.returnUrl) {
        this.router.navigate([this.returnUrl]);
      }
      else {
        this.router.navigate(["/"]);
      }
    },
      (err: HttpErrorResponse) => {
        errorCallback(err);
      });;
  }

  registerUser(user, successCallback = (data: any) => {}, errorCallback = (err: HttpErrorResponse) => { }) {
    var reqHeader = new HttpHeaders({ 'Content-Type': 'application/json ', 'No-Auth': 'True' });
    this.http.post(this.apiHost + '/api/accounts', user, { headers: reqHeader }).subscribe((data: any) => {
      successCallback(data);
    },
      (err: HttpErrorResponse) => {
        errorCallback(err);
      });;
  }

  updateUserClaims(): Observable<any> {
    this.logger.info("Updating user claims");
    return this.http.get(this.apiHost + '/api/accounts');
  }

  logout(){
    sessionStorage.removeItem(UserService.userToken);
    this.router.navigate(['/signin']);
  }
}
