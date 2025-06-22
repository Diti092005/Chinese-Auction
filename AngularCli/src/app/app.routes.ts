import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { SignUpComponent } from './components/sign-up-component/sign-up-component';
import { CategoryComponent } from './components/category/category';
import { DonorComponent } from './components/donor/donor';

export const routes: Routes = [

    { path: 'signup', component: SignUpComponent },
    { path: 'login', component: LoginComponent },
    { path: '', component: LoginComponent },
    { path: 'categories', component: CategoryComponent },
    {path:'donors',component:DonorComponent}
    // ...שאר הנתיבים
];

