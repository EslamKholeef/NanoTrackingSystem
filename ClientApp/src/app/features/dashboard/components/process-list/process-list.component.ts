import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Process } from 'src/app/core/models/workflow.models';
import { WorkflowService } from 'src/app/core/services/workflow.service';

@Component({
  selector: 'app-process-list',
  templateUrl: './process-list.component.html',
  styleUrls: ['./process-list.component.scss']
})
export class ProcessListComponent implements OnInit {
  processes: Process[] = [];
  loading = false;
  filterForm: FormGroup;
  
  displayedColumns = ['id', 'workflowName', 'initiator', 'status', 'currentStep', 'startedAt', 'actions'];
  
  statusOptions = [
    { value: '', label: 'All Statuses' },
    { value: 'Active', label: 'Active' },
    { value: 'Completed', label: 'Completed' },
    { value: 'Pending', label: 'Pending' },
    { value: 'Rejected', label: 'Rejected' }
  ];

  constructor(
    private fb: FormBuilder,
    private workflowService: WorkflowService,
    private snackBar: MatSnackBar
  ) {
    this.filterForm = this.fb.group({
      status: [''],
      workflowId: ['']
    });
  }

  ngOnInit(): void {
    this.loadProcesses();
    
    this.filterForm.valueChanges.subscribe(() => {
      this.loadProcesses();
    });
  }

  loadProcesses(): void {
    this.loading = true;
    
    const filters = this.filterForm.value;
    const params: any = {};
    
    if (filters.status) params.status = filters.status;
    if (filters.workflowId) params.workflowId = filters.workflowId;
    
    this.workflowService.getProcesses(params).subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success) {
          this.processes = response.processes || [];
        }
      },
      error: (error) => {
        this.loading = false;
        this.snackBar.open('Failed to load processes', 'Close', { duration: 3000 });
      }
    });
  }

  startNewProcess(): void {
    const workflowId = prompt('Enter Workflow ID to start:');
    
    if (workflowId && !isNaN(Number(workflowId))) {
      const request = {
        workflowId: Number(workflowId)
      };

      this.workflowService.startProcess(request).subscribe({
        next: (response) => {
          if (response.success) {
            this.snackBar.open('Process started successfully!', 'Close', { duration: 3000 });
            this.loadProcesses();
          } else {
            this.snackBar.open(response.message, 'Close', { duration: 5000 });
          }
        },
        error: (error) => {
          this.snackBar.open('Failed to start process', 'Close', { duration: 5000 });
        }
      });
    }
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'active': return 'primary';
      case 'completed': return 'accent';
      case 'rejected': return 'warn';
      default: return 'basic';
    }
  }
}