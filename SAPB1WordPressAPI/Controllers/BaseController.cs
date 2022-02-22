using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAPB1WordPressAPI.DataModel.Bases;
using SAPB1WordPressAPI.DataModel.DAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAPB1WordPressAPI.Web.Controllers
{
    public class BaseController : ControllerBase
    {
        public UnitOfWork _uow { get; set; }

        public BaseController(DbContext context)
        {
            _uow = new UnitOfWork(context);
        }

        #region Create
        public async Task<ActionResult<T>> CreateAsync<T, TU>(T entity)
            where T : BaseEntity, new()
            where TU : AbstractValidator<T>
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var validator = (AbstractValidator<T>)Activator.CreateInstance(typeof(TU), new object[] { _uow });
                var validations = _uow.GetGenericRepository<T>().Insert(entity, validator);

                if (validations.IsValid)
                {
                    await _uow.SaveAsync();
                    return Ok(entity);
                }

                return BadRequest(validations.ToString("|"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        public ActionResult<T> Create<T, TU>(T entity)
            where T : BaseEntity, new()
            where TU : AbstractValidator<T>
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var validator = (AbstractValidator<T>)Activator.CreateInstance(typeof(TU), new object[] { _uow });
                var validations = _uow.GetGenericRepository<T>().Insert(entity, validator);

                if (validations.IsValid)
                {
                    _uow.Save();
                    return Ok(entity);
                }
                return BadRequest(validations.ToString("|"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        public ActionResult<IEnumerable<T>> Create<T, TU>(IEnumerable<T> entities)
            where T : BaseEntity, new()
            where TU : AbstractValidator<T>
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var validator = (AbstractValidator<T>)Activator.CreateInstance(typeof(TU), new object[] { _uow });

                foreach (var entity in entities)
                {
                    var validations = _uow.GetGenericRepository<T>().Insert(entity, validator);

                    if (!validations.IsValid)
                    {
                        return BadRequest(validations.ToString("|"));
                    }
                }

                _uow.Save();

                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        public async Task<ActionResult<IEnumerable<T>>> CreateAsync<T, TU>(IEnumerable<T> entities)
            where T : BaseEntity, new()
            where TU : AbstractValidator<T>
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var validator = (AbstractValidator<T>)Activator.CreateInstance(typeof(TU), new object[] { _uow });

                foreach (var entity in entities)
                {
                    var validations = _uow.GetGenericRepository<T>().Insert(entity, validator);

                    if (!validations.IsValid)
                    {
                        return BadRequest(validations.ToString("|"));
                    }
                }

                await _uow.SaveAsync();

                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        #endregion

        #region Update

        public ActionResult<T> Update<T, TU>(T entity)
            where T : BaseEntity, new()
            where TU : AbstractValidator<T>
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var validator = (AbstractValidator<T>)Activator.CreateInstance(typeof(TU), new object[] { _uow });
                var validations = _uow.GetGenericRepository<T>().Update(entity, validator);

                if (validations.IsValid)
                {
                    _uow.Save();
                    return Ok(entity);
                }

                return BadRequest(validations.ToString("|"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        public async Task<ActionResult<T>> UpdateAsync<T, TU>(T entity)
            where T : BaseEntity, new()
            where TU : AbstractValidator<T>
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var validator = (AbstractValidator<T>)Activator.CreateInstance(typeof(TU), new object[] { _uow });
                var validations = _uow.GetGenericRepository<T>().Update(entity, validator);

                if (validations.IsValid)
                {
                    await _uow.SaveAsync();
                    return Ok(entity);
                }

                return BadRequest(validations.ToString("|"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        #endregion

        #region Delete

        public ActionResult<T> Delete<T>(int key) where T : BaseEntity, new()
        {
            try
            {
                _uow.GetGenericRepository<T>().Delete(key);
                _uow.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        public async Task<ActionResult<T>> DeleteAsync<T>(int key) where T : BaseEntity, new()
        {
            try
            {
                _uow.GetGenericRepository<T>().Delete(key);
                await _uow.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        public ActionResult<T> Delete<T>(T entity) where T : BaseEntity, new()
        {
            try
            {
                _uow.GetGenericRepository<T>().Delete(entity);
                _uow.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        public ActionResult<IEnumerable<T>> Delete<T>(IEnumerable<T> entities) where T : BaseEntity, new()
        {
            try
            {
                _uow.GetGenericRepository<T>().Gone(entities);
                _uow.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        public async Task<ActionResult<T>> DeleteAsync<T>(T entity) where T : BaseEntity, new()
        {
            try
            {
                _uow.GetGenericRepository<T>().Delete(entity);
                await _uow.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        #endregion

    }
}
