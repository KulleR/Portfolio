import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { LoggerModule, NgxLoggerLevel, NGXLogger } from 'ngx-logger';
import { NgProgressModule } from '@ngx-progressbar/core';
import { NgProgressHttpModule } from '@ngx-progressbar/http';

import { AppComponent } from './app.component';
import { SpeechRecognitionComponent } from './main/speech-recognition/speech-recognition.component';
import { HomeLayoutComponent } from './layout/home-layout/home-layout.component';
import { AppRoutingModule } from './app.routing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SignInComponent } from './main/sign-in/sign-in.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ShowInputErrorsComponent } from './main/show-input-errors/show-input-errors.component';
import { AuthGuard } from './guards/auth.guard';
import { AuthInterceptor } from './intercepters/auth.intercepter';
import { SignUpComponent } from './main/sign-up/sign-up.component';
import { SignLayoutComponent } from './layout/sign-layout/sign-layout.component';
import { FormValidatorService } from './services/form-validator.service';
import { SortablejsModule } from 'angular-sortablejs/dist';
import { CreateBlockComponent } from './main/create-block/create-block.component';
import { EmptyLayoutComponent } from './layout/empty-layout/empty-layout.component';
import { IsBlockRecognizedFilterPipe } from './pipes/MyFilterPipe';

let config = require('./../../config/web.config.json');

@NgModule({
  declarations: [
    AppComponent,
    SpeechRecognitionComponent,
    HomeLayoutComponent,
    SpeechRecognitionComponent,
    SignInComponent,
    ShowInputErrorsComponent,
    SignUpComponent,
    SignLayoutComponent,
    CreateBlockComponent,
    EmptyLayoutComponent,
    IsBlockRecognizedFilterPipe
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    LoggerModule.forRoot({ serverLoggingUrl: `${config.api.url}/api/logs`, level: NgxLoggerLevel.DEBUG, serverLogLevel: NgxLoggerLevel.OFF }),
    SortablejsModule,
    NgProgressModule.forRoot(),
    NgProgressHttpModule.forRoot()
  ],
  providers: [
    AuthGuard,
    NGXLogger,
    {
      provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true
    },
    FormValidatorService
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

}
