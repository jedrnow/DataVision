import { HttpClient, HttpHeaders, HttpResponseBase, HttpResponse } from "@angular/common/http";
import { Injectable, Inject, Optional } from "@angular/core";
import { API_BASE_URL, SwaggerException } from "../web-api-client";
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, throwError as _observableThrow, of as _observableOf } from 'rxjs';

export interface IReportsDownloader {
    downloadReport(id: number): Observable<Blob>;
}

@Injectable({
    providedIn: 'root'
})
export class ReportsDownloader implements IReportsDownloader {
    private http: HttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ?? "";
    }

    downloadReport(id: number): Observable<Blob> {
        let url_ = this.baseUrl + "/api/Reports/{id}/Download";
        if (id === undefined || id === null)
            throw new Error("The parameter 'id' must be defined.");
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");
    
        let options_: any = {
            observe: "response" as const,
            responseType: "blob" as const,
            headers: new HttpHeaders({
            })
        };
    
        return this.http.request("get", url_, options_).pipe(
            _observableMergeMap((response_: any) => {
                return this.processDownloadReport(response_);
            }),
            _observableCatch((response_: any) => {
                if (response_ instanceof HttpResponseBase) {
                    try {
                        return this.processDownloadReport(response_ as any);
                    } catch (e) {
                        return _observableThrow(e) as any as Observable<Blob>;
                    }
                } else
                    return _observableThrow(response_) as any as Observable<Blob>;
            })
        );
    }
    
    protected processDownloadReport(response: HttpResponseBase): Observable<Blob> {
        const status = response.status;
        const responseBlob =
            response instanceof HttpResponse ? response.body :
            (response as any).error instanceof Blob ? (response as any).error : undefined;
    
        let _headers: any = {}; if (response.headers) { for (let key of response.headers.keys()) { _headers[key] = response.headers.get(key); }}
        if (status === 200) {
            return _observableOf(responseBlob as Blob);
        } else if (status !== 200 && status !== 204) {
            return this.blobToText(responseBlob).pipe(_observableMergeMap((_responseText: string) => {
                return this.throwException("An unexpected server error occurred.", status, _responseText, _headers);
            }));
        }
        return _observableOf(responseBlob as Blob);
    }
    
    private blobToText(blob: any): Observable<string> {
        return new Observable<string>((observer: any) => {
            if (!blob) {
                observer.next("");
                observer.complete();
            } else {
                let reader = new FileReader();
                reader.onload = event => {
                    observer.next((event.target as any).result);
                    observer.complete();
                };
                reader.readAsText(blob);
            }
        });
    }
    
    private throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
        if (result !== null && result !== undefined)
            return _observableThrow(result);
        else
            return _observableThrow(new SwaggerException(message, status, response, headers, null));
    }
}