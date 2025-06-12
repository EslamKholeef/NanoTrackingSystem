import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Process } from 'src/app/core/models/workflow.models';
import { WorkflowService } from 'src/app/core/services/workflow.service';

@Component({
  selector: 'app-my-tasks',
  templateUrl: './my-tasks.component.html',
  styleUrls: ['./my-tasks.component.scss']
})
export class MyTasksComponent implements OnInit {
  tasks: Process[] = [];
  loading = false;
  
  displayedColumns = ['workflowName', 'currentStep', 'startedAt', 'actions'];

  constructor(
    private workflowService: WorkflowService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.loadMyTasks();
  }

  loadMyTasks(): void {
    this.loading = true;
    
    this.workflowService.getMyTasks().subscribe({
      next: (response) => {
        this.loading = false;
        if (response.success) {
          this.tasks = response.processes || [];
        }
      },
      error: (error) => {
        this.loading = false;
        this.snackBar.open('Failed to load tasks', 'Close', { duration: 3000 });
      }
    });
  }

  executeTask(task: Process, action: string): void {
    const comments = prompt(`Comments for ${action}:`);
    
    if (comments !== null) {
      const request = {
        processId: task.id,
        stepName: task.currentStep!,
        action: action,
        comments: comments
      };

      this.workflowService.executeProcess(request).subscribe({
        next: (response) => {
          if (response.success) {
            this.snackBar.open(`Task ${action}d successfully!`, 'Close', { duration: 3000 });
            this.loadMyTasks();
          } else {
            this.snackBar.open(response.message, 'Close', { duration: 5000 });
          }
        },
        error: (error) => {
          this.snackBar.open('Failed to execute task', 'Close', { duration: 5000 });
        }
      });
    }
  }
}
