import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'tableFilter'
})
export class TableFilterPipe implements PipeTransform {
  transform(list: any[], searchText: string, showTrash: boolean) {
    var searchList: any[] = searchText ? list.filter(item => item.name.toLocaleLowerCase().includes(searchText.toLocaleLowerCase())) : list;
    return showTrash ? searchList.filter(item => item.quality == 0) : searchList;
  }
}