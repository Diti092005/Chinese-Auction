import { Component, OnInit } from '@angular/core';
import { Donor } from '../../models/donor.model';
import { DonorService } from '../../services/donor-service';
import { DonorFormComponent } from "../donor-form/donor-form";

import { MessageService, ConfirmationService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';

import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TooltipModule } from 'primeng/tooltip';
import { DonorDto } from '../../models/donor.dto';

@Component({
  selector: 'app-donor',
  templateUrl: './donor.html',
  styleUrls: ['./donor.scss'],
  standalone: true,
  providers: [MessageService, ConfirmationService],
  imports: [
    DonorFormComponent,
    ToastModule,
    ConfirmDialogModule,
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ButtonModule,
    TooltipModule ,
    TableModule,
    InputTextModule
  ],
})
export class DonorComponent implements OnInit {
  donors: any[] = [];
  displayAddDialog = false;
  selectedDonor: Donor | null = null;

  searchName = '';
  searchEmail = '';
  searchGift = '';

  constructor(
    private donorService: DonorService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit() {
    this.loadDonors();
  }

  loadDonors() {
    this.donorService.getAll().subscribe({
      next: (data) => {
        this.donors = data.map((donor) => ({
          ...donor,
          giftsCount: donor.gifts ? donor.gifts.length : 0,
        }));
      },
      error: () => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'Failed to load donors',
        });
      },
    });
  }

  openAddDialog() {
    this.selectedDonor = null;
    this.displayAddDialog = true;
  }

  editDonor(donor: Donor) {
    this.selectedDonor = { ...donor };
    this.displayAddDialog = true;
  }

  deleteDonor(donorId: number) {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete this donor?',
      header: 'Confirm Deletion',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.donorService.delete(donorId).subscribe({
          next: () => {
            this.donors = this.donors.filter((d) => d.id !== donorId);
            this.messageService.add({
              severity: 'success',
              summary: 'Deleted',
              detail: 'Donor deleted successfully',
            });
          },
          error: (err) => {
            let detail = 'Failed to delete donor';
            if (err && err.error && typeof err.error === 'string' && err.error.includes('Cannot delete donor because he has gifts')) {
              detail = 'Cannot delete donor because he has gifts. Please remove all gifts before deleting the donor.';
            } else if (err && err.error && err.error.message && typeof err.error.message === 'string' && err.error.message.includes('Cannot delete donor because he has gifts')) {
              detail = 'Cannot delete donor because he has gifts. Please remove all gifts before deleting the donor.';
            }
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail,
            });
          },
        });
      },
    });
  }

  search() {
    this.donorService
      .search(this.searchName, this.searchEmail, this.searchGift)
      .subscribe({
        next: (data) => (this.donors = data),
        error: (err) => {
          if (err.status === 404) {
            this.donors = [];
          } else {
            this.messageService.add({
              severity: 'error',
              summary: 'Error',
              detail: 'Search failed',
            });
          }
        },
      });
  }

  onSaveDonor(donor: Donor) {
    if (donor.id) {
      const dto: DonorDto = {
        name: donor.name,
        email: donor.email,
        showMe: donor.showMe ?? false
      };
      this.donorService.update(donor.id, dto).subscribe({
        next: () => {
          const index = this.donors.findIndex((d) => d.id === donor.id);
          if (index !== -1) {
            this.donors[index] = {
              ...donor,
              giftsCount: donor.gifts ? donor.gifts.length : 0,
            };
          }
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Donor updated successfully',
          });
          this.displayAddDialog = false;
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to update donor',
          });
        },
      });
    } else {
      this.donorService.add(donor).subscribe({
        next: (addedDonor) => {
          this.donors.push({
            ...addedDonor,
            giftsCount: addedDonor.gifts ? addedDonor.gifts.length : 0,
          });
          this.messageService.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Donor added successfully',
          });
          this.displayAddDialog = false;
        },
        error: () => {
          this.messageService.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to add donor',
          });
        },
      });
    }
  }
}
