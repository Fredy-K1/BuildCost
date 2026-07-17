import { Routes } from '@angular/router';
import {Login} from './Auth/login/login';

export const routes: Routes = [
    { path: '', component: Login },
    { path: 'login', component: Login },
    {path: '**', redirectTo: 'login', pathMatch: 'full'}
];
