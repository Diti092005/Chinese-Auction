import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';



import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { DialogModule } from 'primeng/dialog';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { Donor } from '../../models/donor.model';
import { DonorService } from '../../services/donor-service';

@Component({
  selector: 'app-donor',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    DialogModule,
    ToastModule
  ],
  templateUrl: './donor.html',
  styleUrls: ['./donor.scss'],
  providers: [MessageService]
})
export class DonorComponent implements OnInit {
  donors: Donor[] = [];
  selectedDonor?: Donor;
  displayGiftsDialog: boolean = false;

  // חדש - דיאלוג הוספה
  displayAddDialog: boolean = false;
  newDonorName: string = '';
  newDonorEmail: string = '';

  searchName: string = '';
  searchEmail: string = '';
  searchGift: string = '';

  constructor(private donorService: DonorService, private messageService: MessageService) {}

  ngOnInit() {
    this.loadDonors();
  }

  loadDonors() {
    this.donorService.getAllDonors().subscribe({
      next: (data) => this.donors = data,
      error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to load donors' })
    });
  }

  search() {
    this.donorService.searchDonors(this.searchName, this.searchEmail, this.searchGift).subscribe({
      next: (data) => this.donors = data,
      error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Search failed' })
    });
  }

  showGifts(donor: Donor) {
    this.selectedDonor = donor;
    this.displayGiftsDialog = true;
  }

  // חדש - פתיחת דיאלוג הוספה
  openAddDialog() {
    this.newDonorName = '';
    this.newDonorEmail = '';
    this.displayAddDialog = true;
  }

  // חדש - שמירת תורם חדש
  addDonor() {
    if (!this.newDonorName.trim() || !this.newDonorEmail.trim()) {
      this.messageService.add({ severity: 'warn', summary: 'Warning', detail: 'Name and Email are required' });
      return;
    }

    const newDonor: Donor = {
      id: 0, // יותקן בשרת
      name: this.newDonorName,
      email: this.newDonorEmail,
      gifts: []
    };

    this.donorService.addDonor(newDonor).subscribe({
      next: (donor) => {
        this.donors.push(donor);
        this.displayAddDialog = false;
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Donor added' });
      },
      error: () => this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Failed to add donor' })
    });
  }
}
