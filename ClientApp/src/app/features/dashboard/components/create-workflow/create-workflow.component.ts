import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { WorkflowService } from 'src/app/core/services/workflow.service';

@Component({
  selector: 'app-create-workflow',
  templateUrl: './create-workflow.component.html',
  styleUrls: ['./create-workflow.component.scss']
})
export class CreateWorkflowComponent {

workflowForm: FormGroup;
  loading = false;

  actionTypes = [
    { value: 'input', label: 'Input' },
    { value: 'approve_reject', label: 'Approve/Reject' },
    { value: 'review', label: 'Review' },
    { value: 'complete', label: 'Complete' }
  ];

  roles = ['Admin', 'Manager', 'Finance', 'HR', 'Employee'];

  constructor(
    private fb: FormBuilder,
    private workflowService: WorkflowService,
    private snackBar: MatSnackBar
  ) {
    this.workflowForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      steps: this.fb.array([this.createStepForm()])
    });
  }

  get steps(): FormArray {
    return this.workflowForm.get('steps') as FormArray;
  }

  createStepForm(): FormGroup {
    return this.fb.group({
      stepName: ['', [Validators.required]],
      assignedTo: ['', [Validators.required]],
      actionType: ['', [Validators.required]],
      nextStep: [''],
      requiresValidation: [false],
      validationEndpoint: ['']
    });
  }

  addStep(): void {
    this.steps.push(this.createStepForm());
  }

  removeStep(index: number): void {
    if (this.steps.length > 1) {
      this.steps.removeAt(index);
    }
  }

  onSubmit(): void {
    if (this.workflowForm.valid) {
      this.loading = true;
      
      this.workflowService.createWorkflow(this.workflowForm.value).subscribe({
        next: (response) => {
          this.loading = false;
          if (response.success) {
            this.snackBar.open('Workflow created successfully!', 'Close', { duration: 3000 });
            this.workflowForm.reset();
            this.steps.clear();
            this.steps.push(this.createStepForm());
          } else {
            this.snackBar.open(response.message, 'Close', { duration: 5000 });
          }
        },
        error: (error) => {
          this.loading = false;
          this.snackBar.open('Failed to create workflow', 'Close', { duration: 5000 });
        }
      });
    }
  }

  loadSampleWorkflow(): void {
    this.workflowForm.patchValue({
      name: 'Purchase Approval Process',
      description: 'A workflow to approve purchase requests'
    });

    this.steps.clear();
    
    const sampleSteps = [
      {
        stepName: 'Submit Request',
        assignedTo: 'Employee',
        actionType: 'input',
        nextStep: 'Manager Approval',
        requiresValidation: false,
        validationEndpoint: ''
      },
      {
        stepName: 'Manager Approval',
        assignedTo: 'Manager',
        actionType: 'approve_reject',
        nextStep: 'Finance Approval',
        requiresValidation: true,
        validationEndpoint: 'https://api.nano.simulate.com/validate'
      },
      {
        stepName: 'Finance Approval',
        assignedTo: 'Finance',
        actionType: 'approve_reject',
        nextStep: 'Completed',
        requiresValidation: true,
        validationEndpoint: 'https://api.nano.simulate.com/finance-check'
      }
    ];

    sampleSteps.forEach(step => {
      this.steps.push(this.fb.group(step));
    });
  }
}