export class DateUtilities {

  /**
   * Converts a date to a string in the format 'YYYY-MM-DD'.
   * @param date The date to convert.
   * @returns The date as a string in the format 'YYYY-MM-DD'.
   */
  static toDateOnlyString(date: Date): string {
    return date.toISOString().split('T')[0];
  }
}
