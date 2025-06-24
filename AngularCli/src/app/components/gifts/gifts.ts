import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GiftService } from '../../services/gift.service';
import { Gift } from '../../models/gift.model';
import { RouterModule, Router } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-gifts',
  standalone: true,
  imports: [CommonModule, TableModule, ButtonModule, RouterModule],
  templateUrl: './gifts.html'
})
export class GiftComponent implements OnInit {
  gifts: Gift[] = [];

  constructor(private giftService: GiftService, private router: Router) { }

  // ngOnInit() {
  //   this.giftService.getAllGifts().subscribe((data: Gift[]) => this.gifts = data);
  // }

  ngOnInit() {
    this.giftService.getAllGifts().subscribe((data: any) => {
      console.log(data.$values ?? []);
      
      this.gifts = data.$values ?? [];
    });
  }



  onAddGift() {
    this.router.navigate(['/gifts/add']);
  }

  onEditGift(id: number) {
    this.router.navigate(['/gifts/edit', id]);
  }
}
