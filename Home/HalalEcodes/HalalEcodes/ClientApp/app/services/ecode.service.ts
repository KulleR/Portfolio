import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Ecode } from '../models/ecode';
import 'rxjs/add/operator/map';

@Injectable()
export class EcodeService {

    constructor(private http: HttpClient) { }

    getEcodes() {
        return this.http.get('/api/ecodes')
            .map(res => res as Ecode[]);
    }

    getEcode(id: string) {
        //let myParams = new URLSearchParams();
        //myParams.set('id', id);
        //let options = new RequestOptions({ params: myParams });
        //console.log(options);
        return this.http.get('/api/ecodes/' + id)
            .map(res => res as Ecode);
    }

}
