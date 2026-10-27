import { Component, output } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateCategory } from '../../models/catalog';

@Component({
  imports: [ReactiveFormsModule],
  selector: 'app-category-form',
  styleUrl: './category-form.css',
  templateUrl: './category-form.html',
})
export class CategoryFormComponent {
  readonly saved = output<CreateCategory>();
  readonly cancelled = output<void>();
  protected readonly form;

  constructor(formBuilder: FormBuilder) {
    this.form = formBuilder.nonNullable.group({
      nombre: ['', [Validators.required, Validators.maxLength(100)]],
      descripcion: ['', Validators.maxLength(250)],
    });
  }

  protected submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { nombre, descripcion } = this.form.getRawValue();
    this.saved.emit({ nombre, descripcion: descripcion || null });
  }
}
