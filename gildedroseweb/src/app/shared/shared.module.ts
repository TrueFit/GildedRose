import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { GrMaterialModule } from './gr-material-module';
import { DataProviderService } from './data-provider-service';

@NgModule({
  imports: [
      CommonModule,
      RouterModule,
      GrMaterialModule
  ],
  exports: [
      CommonModule,
      GrMaterialModule,
  ],
  providers: [
    DataProviderService
  ],
})
export class SharedModule { }
