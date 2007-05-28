//

using System;
using System.Collections;
using Gendarme.Framework;
using Mono.Cecil;

namespace MonoDevelop
{
       
    public class GendarmeRunner : Runner
    {
        
        public GendarmeRunner()
        {
        }
        public event ProgressChangedHandler ProgressChanged;                                                                                                     
        public delegate void ProgressChangedHandler(int progress);                                                                                      
        public void OnProgressChanged(int progress){                                                                                                    
            if(ProgressChanged != null)                                                                                                                        
                ProgressChanged(progress);                                                                                                                       
        }        
        int _progress = 0;
        int current_progress {
            get { return _progress; }
            set { 
                _progress = value;
                OnProgressChanged(value);
                }
        }
        public void ProcessWithProgress (AssemblyDefinition assembly){
           _CheckAssembly (assembly);                                                                                                            
           current_progress = 0;  
           int module_step = 0;
           if (assembly.Modules.Count>0)
             module_step = (100 / assembly.Modules.Count);
           foreach (ModuleDefinition module in assembly.Modules) {                                                                              
                 _CheckModule (module);                                                                                                        
                 int type_step = 0;
                 if(module.Types.Count>0) 
                    type_step = (module_step / module.Types.Count);
                 else   
                    current_progress += module_step;
                 foreach (TypeDefinition type in module.Types){
                         current_progress += type_step; 
                         _CheckType (type);             
                 }
           }            
           current_progress = 100;
        }
        
          void _CheckAssembly (AssemblyDefinition assembly)                                                                                             
                {                                                                                                                                            
                        foreach (IAssemblyRule rule in Rules.Assembly)                                                                                       
                                _ProcessMessages (rule.CheckAssembly (assembly, this), rule, assembly);                                                       
                }                                                                                                                                            
                                                                                                                                                             
                void _CheckModule (ModuleDefinition module)                                                                                                   
                {                                                                                                                                            
                        foreach (IModuleRule rule in Rules.Module)                                                                                           
                                _ProcessMessages (rule.CheckModule (module, this), rule, module);                                                             
                }                                                                                                                                            
       void _ProcessMessages (MessageCollection messages, IRule rule, object target)                                                                 
                {                                                                                                                                            
                        if (messages == RuleSuccess)                                                                                                         
                                return;                                                                                                                      
                                                                                                                                                             
                        Violations.Add (rule, target, messages);                                                                                             
                }              
                                                                                                                                          
                                                                                                                                                             
                void _CheckType (TypeDefinition type)                                                                                                         
                {                                                                                                                                            
                        foreach (ITypeRule rule in Rules.Type)                                                                                               
                                _ProcessMessages (rule.CheckType (type, this), rule, type);                                                                   
                                                                                                                                                             
                        _CheckMethods (type);                                                                                                                 
                }                                                                                                                                            
                                                                                                                                                             
                void _CheckMethods (TypeDefinition type)                                                                                                      
                {                                                                                                                                            
                        _CheckMethods (type, type.Constructors);                                                                                              
                        _CheckMethods (type, type.Methods);                                                                                                   
                }                                                                                                                                            
                                                                                                                                                             
                void _CheckMethods (TypeDefinition type, ICollection methods)                                                                                 
                {                                                                                                                                            
                        foreach (MethodDefinition method in methods)                                                                                         
                                foreach (IMethodRule rule in Rules.Method)                                                                                   
                                        _ProcessMessages (rule.CheckMethod (method, this), rule, method);                                                     
                }                                   
    }
}
