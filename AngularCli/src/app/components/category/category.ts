import { Component, OnInit } from '@angular/core';
import { CategoryService } from '../../services/category';
import { Category } from '../../models/category.model';
import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { TableModule } from 'primeng/table';  // <-- חשוב לייבא
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-category',
  templateUrl: './category.html',
  styleUrls: ['./category.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, TableModule, ButtonModule, InputTextModule]
})
export class CategoryComponent implements OnInit {
  categories: Category[] = [];
  newCategoryName: string = '';
  loading: boolean = false;
  message: string = '';
  errorMessage: string = '';

  ngOnInit(): void {
    this.getAllCategories();
  }

  constructor(private categoryservice: CategoryService) { }

  getAllCategories() {
    this.categoryservice.getAllCategories().subscribe({
      next: data => {
        this.categories = data;
      },
      error: err => {
        this.errorMessage = 'Failed to load categories.';
      }
    });
  }

  addCategory() {
    const newCategory = this.newCategoryName.trim();
    if (!newCategory) {
      this.errorMessage = 'Category name cannot be empty.';
      this.message = '';
      return;
    }

    this.loading = true;
    this.message = '';
    this.errorMessage = '';

    this.categoryservice.addCategory(newCategory).subscribe({
      next: category => {
        this.categories.push(category);
        this.newCategoryName = '';
        this.message = 'Category added successfully!';
        this.loading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.loading = false;
        if (error.status === 409) {
          this.errorMessage = 'Category already exists.';
        } else {
          this.errorMessage = 'Unexpected error occurred. Please try again.';
        }
      }
    });
  }


  saveCategory(category: Category) {
    if (!category.name.trim()) {
      alert('Category name cannot be empty.');
      return;
    }
    this.categoryservice.updateCategory(category.name, category.id).subscribe({
      next: () => {
        this.message = 'Category updated successfully!';
        this.errorMessage = '';
      },
      error: () => {
        alert('Error updating category.');
      }
    });
  }

  deleteCategory(category: Category) {
    if (!confirm(`Are you sure you want to delete category "${category.name}"?`)) {
      return;
    }
    this.categoryservice.deleteCategory(category.id).subscribe({
      next: () => {
        this.categories = this.categories.filter(c => c.id !== category.id);
        this.message = 'Category deleted successfully!';
        this.errorMessage = '';
      },
      error: () => {
        alert('Error deleting category.');
      }
    });
  }
}
