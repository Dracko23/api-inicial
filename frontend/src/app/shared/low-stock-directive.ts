import { Directive, HostBinding, Input } from '@angular/core';

@Directive({
  selector: '[appLowStock]',
  standalone: true,
})
export class LowStockDirective {
  @Input() appLowStock = 0;

  @HostBinding('class.stock-low')
  get isLowStock(): boolean {
    return this.appLowStock < 5;
  }
}
