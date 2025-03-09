// src/app/components/products/products.component.ts
import { Component, OnInit } from '@angular/core';
import { ProductService } from '../app/services/product.service';
import { Product } from '../app/models/product';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  products: Product[] = [];
  selectedProduct?: Product;
  loading = false;
  displayedColumns: string[] = ['name', 'price', 'description', 'createdDate', 'actions'];

  productForm = new FormGroup({
    id: new FormControl(0),
    name: new FormControl('', [Validators.required]),
    price: new FormControl(0, [Validators.required, Validators.min(0)]),
    description: new FormControl('', [Validators.required])
  });

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    this.productService.getAllProducts().subscribe({
      next: (data) => {
        this.products = data;
        this.loading = false;
      },
      error: (error) => {
        console.error(error);
        this.loading = false;
      }
    });
  }

  createProduct(): void {
    const productData: Omit<Product, 'id'> = {
      name: this.productForm.value.name!,
      price: this.productForm.value.price!,
      description: this.productForm.value.description!,
      createdDate: new Date().toISOString()
    };
    this.productService.createProduct(productData).subscribe({
      next: () => {
        this.loadProducts();
        this.resetForm();
      },
      error: (error) => console.error(error)
    });
  }

  updateProduct(id: number): void {
    const productData: Product = {
      id: this.productForm.value.id!,
      name: this.productForm.value.name!,
      price: this.productForm.value.price!,
      description: this.productForm.value.description!,
      createdDate: this.selectedProduct?.createdDate || new Date().getDate().toString() 
    };
    this.productService.updateProduct(id, productData).subscribe({
      next: () => {
        this.loadProducts();
        this.resetForm();
      },
      error: (error) => console.error(error)
    });
  }


  deleteProduct(id: number): void {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productService.deleteProduct(id).subscribe({
        next: () => this.loadProducts(),
        error: (error) => console.error(error)
      });
    }
  }

  resetForm(): void {
     
    this.productForm.markAsPristine();
    this.productForm.markAsUntouched();
    this.productForm.updateValueAndValidity();
  }

  selectProduct(product: Product): void {
    this.selectedProduct = product;
    this.productForm.patchValue(product);
  }
}
