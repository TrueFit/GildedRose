import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { GrMaterialModule } from './gr-material-module';

@NgModule({
  imports: [
      CommonModule,
      RouterModule,
      GrMaterialModule
  ],
  exports: [
      CommonModule,
      GrMaterialModule,
  ]
})
export class SharedModule { }
