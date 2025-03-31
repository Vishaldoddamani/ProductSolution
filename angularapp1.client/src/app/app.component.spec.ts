import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { of } from 'rxjs';
import { AppComponent } from './app.component';
import { ProductService } from '../app/services/product.service';
import { Product } from '../app/models/product';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let productService: ProductService;
  let httpMock: HttpTestingController;

  const mockProducts: Product[] = [
    { id: 1, name: 'Product 1', price: 100, description: 'Description 1', createdDate: '2021-10-01' },
    { id: 2, name: 'Product 2', price: 200, description: 'Description 2', createdDate: '2021-10-02' }
  ];

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      imports: [
        HttpClientTestingModule,
        ReactiveFormsModule
      ],
      providers: [ProductService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    productService = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
    fixture.detectChanges();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should load products on initialization', () => {
    spyOn(productService, 'getAllProducts').and.returnValue(of(mockProducts));
    component.ngOnInit();
    expect(component.products).toEqual(mockProducts);
  });

  it('should create a product', () => {
    const newProduct: Omit<Product, 'id'> = {
      name: 'New Product',
      price: 300,
      description: 'New Description',
      createdDate: new Date().toISOString()
    };

    spyOn(productService, 'createProduct').and.returnValue(of({ ...newProduct, id: 3 }));
    spyOn(component, 'loadProducts');

    component.productForm.setValue({
      id: 0,
      name: newProduct.name,
      price: newProduct.price,
      description: newProduct.description
    });

    component.createProduct();
    expect(productService.createProduct).toHaveBeenCalledWith(newProduct);
    expect(component.loadProducts).toHaveBeenCalled();
  });

  it('should update a product', () => {
    const updatedProduct: Product = {
      id: 1,
      name: 'Updated Product',
      price: 150,
      description: 'Updated Description',
      createdDate: '2021-10-01'
    };

    spyOn(productService, 'updateProduct').and.returnValue(of(updatedProduct));
    spyOn(component, 'loadProducts');

    component.productForm.setValue({
      id: updatedProduct.id,
      name: updatedProduct.name,
      price: updatedProduct.price,
      description: updatedProduct.description
    });

    component.updateProduct(updatedProduct.id);
    expect(productService.updateProduct).toHaveBeenCalledWith(updatedProduct.id, updatedProduct);
    expect(component.loadProducts).toHaveBeenCalled();
  });

  it('should delete a product', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    spyOn(productService, 'deleteProduct').and.returnValue(of({}));
    spyOn(component, 'loadProducts');

    component.deleteProduct(1);
    expect(productService.deleteProduct).toHaveBeenCalledWith(1);
    expect(component.loadProducts).toHaveBeenCalled();
  });

  it('should select a product', () => {
    const product = mockProducts[0];
    component.selectProduct(product);
    expect(component.selectedProduct).toEqual(product);
    expect(component.productForm.value).toEqual({
      id: product.id,
      name: product.name,
      price: product.price,
      description: product.description
    });
  });

  it('should reset the form', () => {
    component.resetForm();
    expect(component.selectedProduct).toBeUndefined();
    expect(component.productForm.pristine).toBeTrue();
    expect(component.productForm.untouched).toBeTrue();
  });
});
