import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { SignUpComponent } from './components/sign-up-component/sign-up-component';
import { CategoryComponent } from './components/category/category';
import { DonorComponent } from './components/donor/donor';
import { Layout } from './components/layout/layout';
import { HomeComponent } from './components/home/home';
import { GiftComponent } from './components/gifts/gifts';
import { GiftAddComponent } from './components/gifts/gift-add/gift-add';
import { GiftListComponent } from './components/gift-list-component/gift-list-component';
import { CreditPaymentComponent } from './components/credit-payment-component/credit-payment-component';

export const routes: Routes = [

  // דף ברירת מחדל - הפניה ל־login
  { path: '', redirectTo: 'login', pathMatch: 'full' },


  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignUpComponent },

  // דפים עם תפריט + פוטר
  {
    path: '',
    component: Layout,
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'categories', component: CategoryComponent },
      { path: 'donors', component: DonorComponent },
      { path: 'gifts', component: GiftComponent },
      { path: 'gifts/add', component: GiftAddComponent },
      { path: "giftslist", component: GiftListComponent },
      {path:"payment-card",component:CreditPaymentComponent}
    ]
  },

  // כתובת לא קיימת → דף הבית
  { path: '**', redirectTo: 'home' }
]

