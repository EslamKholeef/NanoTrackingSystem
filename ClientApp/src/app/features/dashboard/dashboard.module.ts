import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './components/dashboard/dashboard.component';

import { ReactiveFormsModule } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule, MatSelectTrigger } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatChip, MatChipListbox, MatChipsModule } from '@angular/material/chips';
import { MatTabsModule } from '@angular/material/tabs';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { DashboardRoutingModule } from './dashboard-routing.module';
import { CreateWorkflowComponent } from './components/create-workflow/create-workflow.component';
import { MyTasksComponent } from './components/my-tasks/my-tasks.component';
import { ProcessListComponent } from './components/process-list/process-list.component';
import { WorkflowListComponent } from './components/workflow-list/workflow-list.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatMenuModule } from '@angular/material/menu';
import { MatToolbarModule } from '@angular/material/toolbar';


@NgModule({
  declarations: [
    DashboardComponent,
    CreateWorkflowComponent,
    MyTasksComponent,
    ProcessListComponent,
    WorkflowListComponent
  ],
  imports: [
    CommonModule,

    DashboardRoutingModule,
    ReactiveFormsModule,
    MatCardModule,
    MatTableModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDialogModule,
    MatIconModule,
    MatChipsModule,
    MatTabsModule,
    MatExpansionModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatToolbarModule,
    MatCheckboxModule,
    MatMenuModule,
  ]
})
export class DashboardModule { }
