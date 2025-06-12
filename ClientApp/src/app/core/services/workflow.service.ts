import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateWorkflowRequest, StartProcessRequest, ExecuteProcessRequest, Process } from '../models/workflow.models';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WorkflowService {
  private readonly API_URL = environment.apiBaseUrl; 
  
  constructor(private http: HttpClient) {}

  createWorkflow(request: CreateWorkflowRequest): Observable<any> {
    return this.http.post(`${this.API_URL}/workflows`, request);
  }

  startProcess(request: StartProcessRequest): Observable<any> {
    return this.http.post(`${this.API_URL}/processes/start`, request);
  }

  executeProcess(request: ExecuteProcessRequest): Observable<any> {
    return this.http.post(`${this.API_URL}/processes/execute`, request);
  }

  getProcesses(params?: any): Observable<any> {
    return this.http.get(`${this.API_URL}/processes`, { params });
  }

  getMyTasks(): Observable<any> {
    return this.http.get(`${this.API_URL}/processes/my-tasks`);
  }

   getWorkflows(): Observable<any> {
    return this.http.get(`${this.API_URL}/workflows`);
  }

  getWorkflow(id: number): Observable<any> {
    return this.http.get(`${this.API_URL}/workflows/${id}`);
  }

  updateWorkflow(id: number, request: any): Observable<any> {
    return this.http.put(`${this.API_URL}/workflows/${id}`, request);
  }

  deleteWorkflow(id: number): Observable<any> {
    return this.http.delete(`${this.API_URL}/workflows/${id}`);
  }
}