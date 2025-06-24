import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { CardModule } from 'primeng/card';
import { MenubarModule } from 'primeng/menubar';
import { PanelModule } from 'primeng/panel';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet,
    MenubarModule,
    CommonModule,
    RouterModule,
    CardModule,
    PanelModule
  ],
  templateUrl: './layout.html',
  styleUrl: './layout.scss'
})
export class Layout {


  items: MenuItem[] = [];

  ngOnInit() {
    this.items = [
      { label: 'Home', icon: 'pi pi-home', routerLink: '/home' },
      { label: 'Gifts', icon: 'pi pi-gift', routerLink: '/gifts' },
      { label: "Donors", icon: " pi pi-users", routerLink: '/donors' },
      { label: "Categories", icon: " pi pi-list", routerLink: '/categories' },
      { label: "ListGiftsToUser", icon: " pi pi-list", routerLink: '/giftslist' },


    ];
  }

}
