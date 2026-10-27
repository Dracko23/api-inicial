import { Component, input, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Category, CreateProduct, Product } from '../../models/catalog';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-product-form',
  styleUrl: './product-form.css',
  templateUrl: './product-form.html',
})
export class ProductFormComponent {
  readonly product = input<Product | null>(null);
  readonly categories = input.required<Category[]>();
  readonly saved = output<CreateProduct>();
  readonly cancelled = output<void>();

  protected readonly form;

  constructor(formBuilder: FormBuilder) {
    this.form = formBuilder.nonNullable.group({
      nombre: ['', [Validators.required, Validators.maxLength(100)]],
      precio: [0, [Validators.required, Validators.min(0), Validators.max(99999999.99)]],
      stock: [0, [Validators.required, Validators.min(0)]],
      estado: [true],
      categoriaId: [0, [Validators.required, Validators.min(1)]],
    });

    const currentProduct = this.product();
    if (currentProduct) {
      this.form.patchValue({
        nombre: currentProduct.nombre,
        precio: currentProduct.precio,
        stock: currentProduct.stock,
        estado: currentProduct.estado,
        categoriaId: currentProduct.categoriaId,
      });
    }
  }

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saved.emit(this.form.getRawValue());
  }
}
