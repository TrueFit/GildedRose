import { Pipe, PipeTransform } from '@angular/core';
import { InventoryItem } from './interfaces';
/*
 * Raise the value exponentially
 * Takes an exponent argument that defaults to 1.
 * Usage:
 *   value | exponentialStrength:exponent
 * Example:
 *   {{ 2 | exponentialStrength:10 }}
 *   formats to: 1024
*/
@Pipe({name: 'valueChanged'})
export class ValueChangedPipe implements PipeTransform {
  transform(value: number | undefined): string {
    return value ? '(' + (value > 0 ? '+' + value : value) + ')' : '';
  }
}

@Pipe({name: 'color'})
export class ColorPipe implements PipeTransform {
	transform(value: number | undefined): string {
		return 'color: ' + (value ? value > 0 ? 'green' : 'red' : 'white');
	}
}
