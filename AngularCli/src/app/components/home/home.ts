import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [CommonModule, CardModule],
 templateUrl: './home.html',
})
export class HomeComponent {}
