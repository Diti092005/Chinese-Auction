import { Component, EventEmitter, Input, Output, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CheckboxModule } from 'primeng/checkbox';

@Component({
  selector: 'app-donor-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    DialogModule,
    InputTextModule,
    ButtonModule,
    CheckboxModule
  ],
  templateUrl: './donor-form.html',
  styleUrls: ['./donor-form.scss']
})
export class DonorFormComponent implements OnChanges {
  @Input() visible = false;
  @Input() donor: any = null; // אפשר לשנות לטיפוס Donor אם יש
  @Output() visibleChange = new EventEmitter<boolean>();
  @Output() save = new EventEmitter<any>();

  donorForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.donorForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      showMe: [true],
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['donor'] && this.donor) {
      // טען את הנתונים שקיבלת ל־form
      this.donorForm.patchValue({
        name: this.donor.name || '',
        email: this.donor.email || '',
        showMe: this.donor.showMe ?? false,
      });
    }
    if (changes['visible'] && !this.visible) {
      // איפוס הטופס כשמסך נסגר
      this.donorForm.reset({ showMe: false });
    }
  }

  get name() {
    return this.donorForm.get('name');
  }
  get email() {
    return this.donorForm.get('email');
  }

  submit() {
    if (this.donorForm.invalid) {
      this.donorForm.markAllAsTouched();
      return;
    }
    // שלח את הערכים יחד עם המזהה אם קיים (לעריכה)
    this.save.emit({ ...this.donor, ...this.donorForm.value });
    this.close();
  }

  close() {
    this.visibleChange.emit(false);
  }
}
