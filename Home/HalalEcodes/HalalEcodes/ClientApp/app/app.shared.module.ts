import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgProgressModule, NgProgressInterceptor } from 'ngx-progressbar';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { EcodeService } from './services/ecode.service';
import { AppComponent } from './components/app/app.component';
import { EcodeListComponent } from './components/ecode-list/ecode-list.component';
import { EcodeComponent } from './components/ecode/ecode.component';

@NgModule({
    declarations: [
        AppComponent,
        EcodeListComponent,
        EcodeComponent
    ],
    imports: [
        HttpClientModule,
        NgProgressModule,
        CommonModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'ecodes', pathMatch: 'full' },
            { path: 'ecodes', component: EcodeListComponent },
            { path: 'ecodes/:code', component: EcodeComponent },
            { path: '**', redirectTo: 'ecodes' }
        ])
    ],
    providers: [
        EcodeService,
        { provide: HTTP_INTERCEPTORS, useClass: NgProgressInterceptor, multi: true }
    ]
})
export class AppModuleShared {
}
