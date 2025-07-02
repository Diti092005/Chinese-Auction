import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { Router, NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { Auth } from '../../services/auth.service';

import { AvatarModule } from 'primeng/avatar';
import { PopoverModule } from 'primeng/popover';
import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    AvatarModule,
    PopoverModule,
    MenubarModule,
    ButtonModule,
    RouterModule
  ],
  templateUrl: './layout.html',
  styleUrls: ['./layout.scss']
})
export class Layout implements OnInit {

  items: MenuItem[] = [];
  initials: string = '';
  fullName: string = '';
  currentUrl: string = '';
  userRole: string = '';

  constructor(private auth: Auth, private router: Router) { }

  ngOnInit() {
    this.userRole = this.auth.getUserRole() || '';
    this.updateMenuItems(this.router.url);

    this.router.events
      .pipe(filter(event => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.currentUrl = event.urlAfterRedirects;
        this.updateMenuItems(this.currentUrl);
      });

    this.fullName = this.auth.getFullName() || '';

    if (this.fullName) {
      const parts = this.fullName.trim().split(' ');
      this.initials = parts.map(p => p.charAt(0)).join('').toUpperCase();
    }

  }

  updateMenuItems(currentUrl: string) {
    if (this.userRole === 'Manager') {
      this.items = [
        { label: 'Home', icon: 'pi pi-home', routerLink: '/home', styleClass: currentUrl.includes('/home') ? 'active-tab' : '' },
        // { label: 'Purchase Tickets', icon: 'pi pi-credit-card', routerLink: '/purchase', styleClass: currentUrl.includes('/purchase') ? 'active-tab' : '' },
        { label: 'Gifts_Manager', icon: 'pi pi-gift', routerLink: '/giftsManager', styleClass: currentUrl.includes('/giftsManager') ? 'active-tab' : '' },
        { label: 'Donors', icon: 'pi pi-users', routerLink: '/donors', styleClass: currentUrl.includes('/donors') ? 'active-tab' : '' },
        { label: 'Categories', icon: 'pi pi-list', routerLink: '/categories', styleClass: currentUrl.includes('/categories') ? 'active-tab' : '' },
        // { label: 'My Tickets', routerLink: '/usertickets', styleClass: currentUrl.includes('/usertickets') ? 'active-tab' : '' },
        { label: 'Raffle', routerLink: '/raffle', styleClass: currentUrl.includes('/raffle') ? 'active-tab' : '' },
        { label: 'Summary', routerLink: '/summary', styleClass: currentUrl.includes('/summary') ? 'active-tab' : '' },
        { label: 'LogOut', routerLink: '/logout', styleClass: currentUrl.includes('/logout') ? 'active-tab' : '' },

      ];
    } else if (this.userRole === 'User') {
      this.items = [
        { label: 'Home', icon: 'pi pi-home', routerLink: '/home', styleClass: currentUrl.includes('/home') ? 'active-tab' : '' },
        { label: 'Purchase Tickets', icon: 'pi pi-credit-card', routerLink: '/purchase', styleClass: currentUrl.includes('/purchase') ? 'active-tab' : '' },
        { label: 'List Gifts', routerLink: '/giftslist', styleClass: currentUrl.includes('/giftslist') ? 'active-tab' : '' },
        { label: 'My Tickets', routerLink: '/usertickets', styleClass: currentUrl.includes('/usertickets') ? 'active-tab' : '' },
        { label: 'LogOut', routerLink: '/logout', styleClass: currentUrl.includes('/logout') ? 'active-tab' : '' },

      ];
    }
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}