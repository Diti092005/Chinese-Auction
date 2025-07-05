import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';

import { GiftService } from '../../../services/gift.service';
import { CategoryService } from '../../../services/category.service';
import { DonorService } from '../../../services/donor-service';
import { Category } from '../../../models/category.model';
import { Donor } from '../../../models/donor.model';

@Component({
  selector: 'app-gift-add',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    InputTextModule,
    DropdownModule,
    ButtonModule,
    RouterModule
  ],
  templateUrl: './gift-add.html',
  styleUrls: ['./gift-add.scss']
})
export class GiftAddComponent implements OnInit {

  giftForm: FormGroup;
  categories: Category[] = [];
  donors: Donor[] = [];

  categoryOptions: { label: string, value: any }[] = [];
  donorOptions: { label: string, value: any }[] = [];
  message: string = '';
  isError: boolean = false;
  isSubmitting: boolean = false;
  selectedFile: File | null = null;
  previewUrl: string | ArrayBuffer | null = null;

  constructor(
    private fb: FormBuilder,
    private giftService: GiftService,
    private router: Router,
    private categoryService: CategoryService,
    private donorService: DonorService
  ) {
    this.giftForm = this.fb.group({
      giftName: ['', Validators.required],
      categoryId: [null, Validators.required],
      donorId: [null, Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      details: [''],
      imageUrl: ['']
    });
  }

  ngOnInit() {
    this.categoryService.getAllCategories().subscribe((res: Category[]) => {
      this.categories = res;
      this.categoryOptions = this.categories.map(c => ({ label: c.name, value: c.id }));
    });

    this.donorService.getAll().subscribe((res: Donor[]) => {
      this.donors = res;
      this.donorOptions = this.donors.map(d => ({ label: d.name, value: d.id }));
    });
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      const reader = new FileReader();
      reader.onload = e => this.previewUrl = reader.result;
      reader.readAsDataURL(file);
    }
  }

  async onSubmit() {
    this.isSubmitting = true;
    this.isError = false;
    this.message = '';
    try {
      let imageUrl = '';
      if (this.selectedFile) {
        const formData = new FormData();
        formData.append('file', this.selectedFile);
        // העלאת קובץ לשרת
        const uploadRes: any = await this.giftService.uploadImage(formData).toPromise();
        imageUrl = uploadRes.imageUrl;
        this.giftForm.patchValue({ imageUrl });
      }
      // שליחת שאר הנתונים
      await this.giftService.addGift(this.giftForm.value).toPromise();
      this.router.navigate(['giftsManager']); // מעבר אוטומטי לעמוד הצגת כל המתנות
    } catch (err: any) {
      this.isError = true;
      this.message = err.error?.message || 'Error adding gift';
    } finally {
      this.isSubmitting = false;
    }
  }

  onCancel() {
    this.router.navigate(['giftsManager']);
  }

  isInvalid(controlName: string): boolean {
    const control = this.giftForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }
}
