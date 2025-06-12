export interface CreateWorkflowRequest {
  name: string;
  description: string;
  steps: WorkflowStep[];
}

export interface WorkflowStep {
  stepName: string;
  assignedTo: string;
  actionType: string;
  nextStep?: string;
  requiresValidation?: boolean;
  validationEndpoint?: string;
}

export interface StartProcessRequest {
  workflowId: number;
}

export interface ExecuteProcessRequest {
  processId: number;
  stepName: string;
  action: string;
  comments?: string;
}

export interface Process {
  id: number;
  workflowId: number;
  workflowName: string;
  initiatorId: string;
  status: string;
  currentStep?: string;
  startedAt: string;
  completedAt?: string;
}

export interface GetWorkflowsResponse {
  success: boolean;
  message: string;
  workflows: WorkflowDto[];
  totalCount: number;
  page: number;
  pageSize: number;
}

export interface WorkflowDto {
  id: number;
  name: string;
  description?: string;
  isActive: boolean;
  createdAt: string;
  updatedAt?: string;
  stepsCount: number;
  steps: WorkflowStepDto[];
}

export interface WorkflowStepDto {
  id: number;
  stepName: string;
  assignedRole: string;
  actionType: string;
  nextStep?: string;
  order: number;
  requiresValidation: boolean;
  validationEndpoint?: string;
}

export interface GetWorkflowResponse {
  success: boolean;
  message: string;
  workflow?: WorkflowDetailDto;
}

export interface WorkflowDetailDto extends WorkflowDto {
  processes: ProcessSummaryDto[];
}

export interface ProcessSummaryDto {
  id: number;
  initiatorId: string;
  status: string;
  currentStep?: string;
  startedAt: string;
  completedAt?: string;
}