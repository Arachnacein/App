export function getMonthName(date: Date): string {
  return date.toLocaleString('pl-PL', { month: 'long', year: 'numeric'});
}
