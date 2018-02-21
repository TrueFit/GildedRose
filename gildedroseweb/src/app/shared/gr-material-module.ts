import {NgModule} from '@angular/core';
import {
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    MatInputModule,
    MatListModule,
    MatToolbarModule,
    MatMenuModule,
    MatSidenavModule,
    MatTabsModule,
    MatRadioModule,
    MatIconModule,
    MatTableModule,
    MatSortModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatAutocompleteModule,
    MatSnackBarModule,
    MatSlideToggleModule,
    MatSelectModule
} from '@angular/material';
import {FlexLayoutModule} from '@angular/flex-layout';

@NgModule({
    exports: [
      FlexLayoutModule,
      MatButtonModule,
      MatCheckboxModule,
      MatCardModule,
      MatInputModule,
      MatListModule,
      MatToolbarModule,
      MatMenuModule,
      MatSidenavModule,
      MatTabsModule,
      MatRadioModule,
      MatIconModule,
      MatTableModule,
      MatSortModule,
      MatProgressSpinnerModule,
      MatDialogModule,
      MatAutocompleteModule,
      MatSnackBarModule,
      MatSlideToggleModule,
      MatSelectModule
    ]
})
export class GrMaterialModule {
}
