// 

import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login';
import { SignUpComponent } from './components/sign-up-component/sign-up-component';
import { CategoryComponent } from './components/category/category';
import { DonorComponent } from './components/donor/donor';
import { Layout } from './components/layout/layout';
import { HomeComponent } from './components/home/home';
import { GiftComponent } from './components/gifts/gift-user/gifts';
import { GiftAddComponent } from './components/gifts/gift-add/gift-add';
import { CreditPaymentComponent } from './components/credit-payment-component/credit-payment-component';
import { GiftManagerComponent } from './components/gifts/gifts-manager';
import { PendingTicketsComponent } from './components/pending-ticket/pending-ticket.component';
import { RoleGuard } from './roleGuard';
import { GiftListComponent } from './components/gift-list/gift-list.component';
import { UserTicketsComponent } from './components/user-tickets/user-tickets.component';
import { RaffleGiftComponent } from './components/raffle-gift/raffle-gift';
import { SummaryComponent } from './components/summary/summary';
import { LogoutComponent } from './components/logout/logout.component';


export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignUpComponent },

  {
    path: '',
    component: Layout,
    children: [
      { path: 'home', component: HomeComponent },

      // Admin-only
      { path: 'categories', component: CategoryComponent, canActivate: [RoleGuard], data: { roles: ['Manager'] } },
      { path: 'donors', component: DonorComponent, canActivate: [RoleGuard], data: { roles: ['Manager'] } },
      { path: 'gifts/add', component: GiftAddComponent, canActivate: [RoleGuard], data: { roles: ['Manager'] } },
      { path: 'giftsManager', component: GiftManagerComponent, canActivate: [RoleGuard], data: { roles: ['Manager'] } },
      { path: 'raffle', component: RaffleGiftComponent, canActivate: [RoleGuard], data: { roles: ['Manager'] } },
      { path: 'summary', component: SummaryComponent, canActivate: [RoleGuard], data: { roles: ['Manager'] } },

      // User-only
      { path: 'purchase', component: PendingTicketsComponent, canActivate: [RoleGuard], data: { roles: ['User', 'Manager'] } },
      { path: 'credit-payment', component: CreditPaymentComponent, canActivate: [RoleGuard], data: { roles: ['User', 'Manager'] } },
      { path: 'giftslist', component: GiftListComponent, canActivate: [RoleGuard], data: { roles: ['User', 'Manager'] } },
      { path: 'usertickets', component: UserTicketsComponent, canActivate: [RoleGuard], data: { roles: ['User', 'Manager'] } },
      { path: 'logout', component: LogoutComponent, canActivate: [RoleGuard], data: { roles: ['User', 'Manager'] } },

      // Everyone
      // { path: 'gifts', component: GiftComponent },
      // { path: 'giftslist', component: GiftComponent },
    ]
  },

  { path: '**', redirectTo: 'home' }
];
