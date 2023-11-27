import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'age',
  standalone: true,
})
export class AgePipe implements PipeTransform {
  transform(value: Date | string): string {
    // convert string to Date
    if (typeof value === 'string') {
      value = new Date(value);
    }

    const now = new Date();
    const ageInYears = now.getFullYear() - value.getFullYear();
    const ageInMonths = now.getMonth() - value.getMonth();

    return `${ageInYears} years, ${ageInMonths} months`;
  }
}
