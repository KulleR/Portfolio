import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { HomeLayoutComponent } from './layout/home-layout/home-layout.component';
import { SpeechRecognitionComponent } from './main/speech-recognition/speech-recognition.component';
import { AuthGuard } from './guards/auth.guard';
import { SignInComponent } from './main/sign-in/sign-in.component';
import { SignUpComponent } from './main/sign-up/sign-up.component';
import { SignLayoutComponent } from './layout/sign-layout/sign-layout.component';
import { EmptyLayoutComponent } from './layout/empty-layout/empty-layout.component';
import { CreateBlockComponent } from './main/create-block/create-block.component';

const routes: Routes = [
    {
        path: '', component: HomeLayoutComponent, data: { title: 'VOCA-SALES' }, canActivate: [AuthGuard], children: [
            {
                path: '', component: SpeechRecognitionComponent
            }
        ]
    },
    {
        path: 'signin', component: EmptyLayoutComponent,data: { title: 'Sign IN - VOCA-SALES' }, children: [
            {
                path: '', component: SignInComponent
            }
        ]
    },
    {
        path: 'signup', component: EmptyLayoutComponent, data: { title: 'Sign UP - VOCA-SALES' }, children: [
            {
                path: '', component: SignUpComponent
            }
        ]
    },
    {
        path: 'createblock', component: HomeLayoutComponent, data: { title: 'Create Block - VOCA-SALES' }, canActivate: [AuthGuard], children: [
            {
                path: '', component: CreateBlockComponent
            }
        ]
    },
    { path: '**', redirectTo: '' }
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }