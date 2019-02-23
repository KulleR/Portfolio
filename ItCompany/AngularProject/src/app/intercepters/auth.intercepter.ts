import { Injectable } from '@angular/core';
import { Observable, of, throwError  } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';
import { HttpInterceptor, HttpErrorResponse, HttpHandler, HttpRequest, HttpEvent, HttpHeaders } from '@angular/common/http';
import { NGXLogger } from 'ngx-logger';
import { UserService } from '../services/user.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router, private logger: NGXLogger) { }

  private handleAuthError(err: HttpErrorResponse): Observable<any> {
    //handle your auth error or rethrow
    if (err.status === 401 || err.status === 403) {
      //navigate /delete cookies or whatever
      this.router.navigateByUrl(`/signin`);
      // if you've caught / handled the error, you don't want to rethrow it unless you also want downstream consumers to have to handle it as well.
      return of(err.message);
    }
    return throwError(err);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    
    var token = sessionStorage.getItem(UserService.userToken);
    var reqHeader = new HttpHeaders({ "Authorization": "Bearer " + token, 'Content-Type': 'application/json' });
    const authReq = req.clone({ headers: reqHeader });
    return next.handle(authReq).pipe(catchError(x => this.handleAuthError(x)));
  }
}