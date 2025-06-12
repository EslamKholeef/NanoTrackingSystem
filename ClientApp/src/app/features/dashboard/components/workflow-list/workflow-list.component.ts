import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { WorkflowService } from 'src/app/core/services/workflow.service';

@Component({
  selector: 'app-workflow-list',
  templateUrl: './workflow-list.component.html',
  styleUrls: ['./workflow-list.component.scss']
})
export class WorkflowListComponent implements OnInit {
  workflows: any[] = [];
  loading = false;
  
  displayedColumns = ['id', 'name', 'description', 'stepsCount', 'createdAt', 'isActive', 'actions'];

  constructor(
    private workflowService: WorkflowService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadWorkflows();
  }

  loadWorkflows(): void {
    this.loading = true;
    
    // **🔥 USE REAL API CALL**
    this.workflowService.getWorkflows().subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success) {
          this.workflows = response.workflows || [];
        } else {
          this.snackBar.open(response.message, 'Close', { duration: 5000 });
        }
      },
      error: (error) => {
        this.loading = false;
        this.snackBar.open('Failed to load workflows', 'Close', { duration: 3000 });
        console.error('Error loading workflows:', error);
      }
    });
  }

  startProcess(workflowId: number): void {
    const request = { workflowId: workflowId };
    
    this.workflowService.startProcess(request).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open('Process started successfully!', 'Close', { duration: 3000 });
        } else {
          this.snackBar.open(response.message, 'Close', { duration: 5000 });
        }
      },
      error: (error) => {
        this.snackBar.open('Failed to start process', 'Close', { duration: 5000 });
        console.error('Error starting process:', error);
      }
    });
  }

  toggleWorkflowStatus(workflow: any): void {
    // **🔥 TODO: Implement update workflow API call**
    this.snackBar.open('Update workflow feature coming soon!', 'Close', { duration: 3000 });
  }
}