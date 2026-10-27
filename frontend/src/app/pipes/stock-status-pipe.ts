import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'stockStatus',
  standalone: true,
})
export class StockStatusPipe implements PipeTransform {
  transform(active: boolean): string {
    return active ? 'Activo' : 'Inactivo';
  }
}
