import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'age',
  standalone: true,
})
export class AgePipe implements PipeTransform {
  transform(value: Date | string): string {
    const now = new Date();
    const birthDate = new Date(value);
    let ageInYears = now.getFullYear() - birthDate.getFullYear();
    let ageInMonths = now.getMonth() - birthDate.getMonth();

    if (ageInMonths < 0) {
      ageInYears--;
      ageInMonths += 12;
    }

    return `${ageInYears} years, ${ageInMonths} months`;
  }
}
